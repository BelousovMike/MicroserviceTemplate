import sys
import uuid
import os
import re

SOLUTION_FOLDERS = {
    "src": [
        "src/Microservice.Template.Core/Microservice.Template.Core.csproj",
        "src/Microservice.Template.Infrastructure/Microservice.Template.Infrastructure.csproj",
        "src/Microservice.Template.ServiceDefaults/Microservice.Template.ServiceDefaults.csproj",
        "src/Microservice.Template.UseCases/Microservice.Template.UseCases.csproj",
        "src/Microservice.Template.Web/Microservice.Template.Web.csproj",
        "src/Microservice.Template.AspireHost/Microservice.Template.AspireHost.csproj"
    ],
    "tests": [
        "tests/Microservice.Template.UnitTests/Microservice.Template.UnitTests.csproj",
        "tests/Microservice.Template.FunctionalTests/Microservice.Template.FunctionalTests.csproj",
        "tests/Microservice.Template.IntegrationTests/Microservice.Template.IntegrationTests.csproj",
        "tests/Microservice.Template.Aspire.Tests/Microservice.Template.Aspire.Tests.csproj"
    ]
}

FOLDER_GUID = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}"

def get_project_guid(sln_lines, project_path):
    # Ищет GUID проекта по пути в .sln
    pattern = re.compile(r'Project\(".*"\) = ".*", "' + re.escape(project_path) + r'", "({[A-F0-9\-]+})"', re.IGNORECASE)
    for line in sln_lines:
        m = pattern.search(line)
        if m:
            return m.group(1)
    return None

def main():
    if len(sys.argv) < 2:
        print("Usage: python restore_solution_folders.py <solution.sln>")
        sys.exit(1)
    sln_path = sys.argv[1]
    if not os.path.isfile(sln_path):
        print(f"File not found: {sln_path}")
        sys.exit(1)

    with open(sln_path, "r", encoding="utf-8") as f:
        sln_lines = f.readlines()

    # 1. Найти конец секции проектов
    end_projects_idx = 0
    for i, line in enumerate(sln_lines):
        if line.strip() == "EndProject":
            end_projects_idx = i
    insert_idx = end_projects_idx + 1

    # 2. Добавить solution folders
    folder_guids = {}
    folder_projects = []
    for folder in SOLUTION_FOLDERS:
        guid = str(uuid.uuid4()).upper()
        folder_guids[folder] = guid
        folder_projects.append(f'Project("{FOLDER_GUID}") = "{folder}", "{folder}", "{guid}"
EndProject
')

    # 3. Вставить solution folders после проектов
    sln_lines = sln_lines[:insert_idx] + folder_projects + sln_lines[insert_idx:]

    # 4. Добавить секцию NestedProjects
    # Собрать соответствие проект-GUID -> folder-GUID
    project_to_folder = {}
    for folder, projects in SOLUTION_FOLDERS.items():
        for proj in projects:
            guid = get_project_guid(sln_lines, proj.replace("/", os.sep))
            if guid:
                project_to_folder[guid] = folder_guids[folder]

    # Найти GlobalSection(NestedProjects)
    nested_start = None
    nested_end = None
    for i, line in enumerate(sln_lines):
        if "GlobalSection(NestedProjects)" in line:
            nested_start = i
        if nested_start and "EndGlobalSection" in line:
            nested_end = i
            break

    nested_lines = []
    for proj_guid, folder_guid in project_to_folder.items():
        nested_lines.append(f"\t\t{proj_guid} = {folder_guid}\n")

    if nested_start and nested_end:
        # Заменить существующую секцию
        sln_lines = sln_lines[:nested_start+1] + nested_lines + sln_lines[nested_end:]
    else:
        # Добавить новую секцию перед EndGlobal
        for i, line in enumerate(sln_lines):
            if "EndGlobal" in line:
                sln_lines = sln_lines[:i] + [
                    "\tGlobalSection(NestedProjects) = preSolution\n"
                ] + nested_lines + [
                    "\tEndGlobalSection\n"
                ] + sln_lines[i:]
                break

    # 5. Сохранить .sln
    with open(sln_path, "w", encoding="utf-8") as f:
        f.writelines(sln_lines)

    print(f"Solution folders restored in {sln_path}")

if __name__ == "__main__":
    main() 