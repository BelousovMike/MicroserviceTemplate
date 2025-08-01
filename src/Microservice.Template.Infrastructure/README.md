## Infrastructure Project

В чистой архитектуре вопросы инфраструктуры рассматриваются отдельно от основных бизнес-правил (или доменной модели в DDD).

Единственный проект, который должен содержать код, связанный с EF, файлами, электронной почтой, веб-службами, Azure/AWS/GCP и т.д., - это инфраструктура.

Инфраструктура должна зависеть от `Core`, в котором существуют абстракции (интерфейсы).

Классы инфраструктуры реализуют интерфейсы, найденные в проекте (проектах) `Core` (Use Cases).

Эти реализации подключаются при запуске с помощью DI.
В данном случае с помощью `Microsoft.Extensions.DependencyInjection` и расширений, определенных в каждом проекте.