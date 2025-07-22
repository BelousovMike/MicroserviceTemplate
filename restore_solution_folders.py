import sys
import uuid
import re
from pathlib import Path
from typing import Dict, List

FOLDER_GUID = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}"
SOLUTION_ITEMS = [
    ".editorconfig",
    "Directory.Build.props",
    "Directory.Packages.props"
]
IGNORE_FOLDERS = {"bin", "obj", "Properties", ".vs", ".git", "node_modules"}


def find_solution_folder_guids(sln_lines: List[str]) -> Dict[str, str]:
    """Извлекает имена solution folders и их GUID из .sln файла."""
    folder_guids = {}
    pattern = re.compile(r'Project\("' + re.escape(FOLDER_GUID) + r'"\) = "([^"]+)", "([^"]+)", "({[A-F0-9\-]+})"')
    for line in sln_lines:
        m = pattern.match(line)
        if m:
            name = m.group(1)
            guid = m.group(3)
            folder_guids[name] = guid
    return folder_guids


def find_global_index(sln_lines: List[str]) -> int:
    """Находит индекс строки, где начинается секция Global."""
    for i, line in enumerate(sln_lines):
        if line.strip().startswith("Global"):
            return i
    return -1


def clean_duplicate_endglobalsection(sln_lines: List[str]) -> List[str]:
    """Удаляет дублирующиеся строки EndGlobalSection."""
    cleaned = []
    prev_end = False
    for line in sln_lines:
        if line.strip() == "EndGlobalSection":
            if prev_end:
                continue
            prev_end = True
        else:
            prev_end = False
        cleaned.append(line)
    return cleaned


def collect_solution_items(solution_dir: Path) -> List[str]:
    """Собирает файлы, которые должны быть добавлены в SolutionItems."""
    return [f for f in SOLUTION_ITEMS if (solution_dir / f).is_file()]


def get_projects_in_folder(root: Path, folder_name: str) -> List[str]:
    """Возвращает список путей к проектам .csproj в папке folder_name (рекурсивно, игнорируя служебные/скрытые папки)."""
    projects = []
    search_root = root / folder_name
    if not search_root.exists():
        return projects
    for path in search_root.rglob("*.csproj"):
        if any(part in IGNORE_FOLDERS or part.startswith(".") for part in path.relative_to(root).parts):
            continue
        projects.append(str(path.relative_to(root)).replace('\\', '/'))
    return projects


def get_project_guids_from_sln(sln_lines: List[str]) -> Dict[str, str]:
    """Возвращает словарь: путь к проекту (в нижнем регистре) -> GUID."""
    project_guids = {}
    pattern = re.compile(r'Project\(".*"\) = ".*", "(.*)", "({[A-F0-9\-]+})"', re.IGNORECASE)
    for line in sln_lines:
        m = pattern.match(line)
        if m:
            proj_path = m.group(1).replace('\\', '/').lower()
            guid = m.group(2)
            project_guids[proj_path] = guid
    return project_guids


def remove_old_nested_projects_section(sln_lines: List[str]) -> List[str]:
    """Удаляет старую секцию GlobalSection(NestedProjects) из .sln файла."""
    new_lines = []
    in_nested = False
    for line in sln_lines:
        if "GlobalSection(NestedProjects)" in line:
            in_nested = True
            continue
        if in_nested and line.strip() == "EndGlobalSection":
            in_nested = False
            continue
        if not in_nested:
            new_lines.append(line)
    return new_lines


def insert_nested_projects_section(sln_lines: List[str], nested_lines: List[str]) -> List[str]:
    """Вставляет секцию NestedProjects перед EndGlobal."""
    if not nested_lines:
        return sln_lines
    insert_idx = None
    for i in range(len(sln_lines) - 1, -1, -1):
        if sln_lines[i].strip() == "EndGlobal":
            insert_idx = i
            break
    if insert_idx is not None:
        return (
            sln_lines[:insert_idx]
            + ["\tGlobalSection(NestedProjects) = preSolution\n"]
            + nested_lines
            + ["\tEndGlobalSection\n"]
            + sln_lines[insert_idx:]
        )
    return sln_lines


def update_sln_with_folders_and_items(sln_path: Path):
    solution_dir = sln_path.parent
    with sln_path.open("r", encoding="utf-8") as f:
        sln_lines = f.readlines()

    folder_guids = find_solution_folder_guids(sln_lines)
    folder_projects = []
    global_idx = find_global_index(sln_lines)
    if global_idx == -1:
        print("Invalid .sln file: no Global section found.")
        sys.exit(1)

    # Создаём только верхнеуровневые solution folders
    for folder in ("src", "tests"):
        if folder not in folder_guids:
            guid = str(uuid.uuid4()).upper()
            folder_guids[folder] = guid
            folder_projects.append(f'Project("{FOLDER_GUID}") = "{folder}", "{folder}", "{guid}"\nEndProject\n')

    # SolutionItems
    solution_items = collect_solution_items(solution_dir)
    if "SolutionItems" not in folder_guids and solution_items:
        guid = str(uuid.uuid4()).upper()
        folder_guids["SolutionItems"] = guid
        items_section = ''.join([f'\t\t{item} = {item}\n' for item in solution_items])
        folder_projects.append(
            f'Project("{FOLDER_GUID}") = "SolutionItems", "SolutionItems", "{guid}"\n'
            f'\tProjectSection(SolutionItems) = preProject\n'
            f'{items_section}'
            f'\tEndProjectSection\nEndProject\n'
        )

    # Вставляем solution folders в .sln
    sln_lines = sln_lines[:global_idx] + folder_projects + sln_lines[global_idx:]

    # Собираем проекты, которые уже есть в .sln
    project_guids = get_project_guids_from_sln(sln_lines)

    # Формируем NestedProjects: проекты -> src/tests
    nested_lines = []
    for folder in ("src", "tests"):
        projects = get_projects_in_folder(solution_dir, folder)
        for proj_path in projects:
            proj_path_lower = proj_path.lower()
            guid = project_guids.get(proj_path_lower)
            if guid and folder in folder_guids:
                nested_lines.append(f"\t\t{guid} = {folder_guids[folder]}\n")

    # Удаляем старую секцию NestedProjects и вставляем новую
    sln_lines = remove_old_nested_projects_section(sln_lines)
    sln_lines = insert_nested_projects_section(sln_lines, nested_lines)
    sln_lines = clean_duplicate_endglobalsection(sln_lines)

    with sln_path.open("w", encoding="utf-8") as f:
        f.writelines(sln_lines)

    print(f"Solution folders and items restored in {sln_path}")


def main():
    if len(sys.argv) < 2:
        print("Usage: python restore_solution_folders.py <solution.sln>")
        sys.exit(1)
    sln_path = Path(sys.argv[1])
    if not sln_path.is_file():
        print(f"File not found: {sln_path}")
        sys.exit(1)
    update_sln_with_folders_and_items(sln_path)

if __name__ == "__main__":
    main() 