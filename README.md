# AntiPlagiarism
Этот проект - микросервисная система, которая позволяет студентам отправлять работы, а преподавателям получать отчёты о плагиате.
Архитектура построена на двух сервисах: сервис хранения файлов и сервис проверки, а также маршрутизирующем gateway. Работа написана на C# (.NET)

Комиссарова Юлия БПИ249

Конструирование программного обеспечения
Домашнее задание №3.

Сборка проекта
Требования:
- Docker (20+ версия)
- Docker Compose
- Порты, которые должны быть свободны:
    - 5001 Storage Service (Swagger)
    - 5002 Check Service (Swagger)
    - 8080 API Gateway + Scalar UI

Инструкция по запуску:
- Клонировать репозиторий:
    - git clone https://github.com/Yakomissarova/Software-Design-HW3.git
    - cd AntiPlagiarism

- Поднять всю систему:
docker compose up --build

- После успешного запуска будут доступны:

Storage Service (сохранение файлов)
Swagger UI:

http://localhost:5001/swagger

Check Service (проверка работ и отчёты)
Swagger UI:

http://localhost:5002/swagger

API Gateway (единая точка входа)

http://localhost:8080/

Scalar — визуальная документация по всем API
http://localhost:8080/scalar


1. Общая идея решения

Проект представляет собой микросервисную систему для проверки учебных работ на плагиат. Архитектура организована по принципам чистой архитектуры (DDD):

Entities - доменные сущности, не зависящие от инфраструктуры

UseCases - бизнес-логика (создание сдачи, анализ, отчёты)

Infrastructure - работа с базой данных (EF Core), HTTP-клиенты, миграции

Presentation - REST API контроллеры

Host - настройка сервисов, Swagger

2. Архитектура

- Storage Service:
    - хранит файлы, которые отправляют студенты:
    - сохраняет файл на диск
    - записывает метаданные в SQLite
    - отдаёт файл по id

- Check Service
    - регистрирует сдачу работы
    - вычисляет хэш содержимого файла
    - осуществляет проверку плагиата
    - выдает отчёты

- API Gateway
Маршрутизатор (nginx):

/files -> storage-service

/submissions -> check-service

/assignments -> check-service

/scalar -> scalar UI

3. Идея проверки плагиата

Для определения плагиата используется хэш содержимого (SHA-256):

- При загрузке файла в Check Service:

    - файл читается в память
    - хэш считается
    - файл отправляется в Storage Service
    - создаётся запись Submission с полями:
        - StudentId
        - AssignmentId
        - FileId
        - ContentHash
        - SubmittedAt

- В момент анализа:
    - собираются все сдачи по одному заданию
    - если есть раньше сданная работа с таким же ContentHash, то:
        - similarity = 1
        - isPlagiarism = true
        - details = "same file content submitted earlier"

То есть система определяет плагиат не по оригинальному имени файла, а по фактическому содержимому

4. Пользовательские сценарии микросервисов и технические сценарии обмена данными

    4.1 Сценарий 1: студент отправляет работу
    
   Действия пользователя: cтудент загружает файл ответа на задание

   - Как вызывается:
       - Запрос в Check Service: POST /submissions
   Тело (multipart/form-data):
         - StudentId
         - AssignmentId
         - File

   - Технический обмен данными между сервисами
       - Check Service получает запрос и считывает файл
       - Check Service вычисляет SHA256-хэш содержимого файла -> ContentHash
       - Check Service вызывает Storage Service:
           - сохраняет файл в файловой системе 
           - записывает метаданные в SQLite
           - возвращает FileId
       - Check Service сохраняет новую запись в своей БД
       - На клиент возвращается: 201 Created
     ```
     {
     "submissionId": "<guid>"
     }

   4.2 Сценарий 2: преподаватель проверяет конкретную работу

   Как вызывается: GET /submissions/{id}/analysis
   - Технический обмен данными
        - Check Service находит Submission по ID в SQLite
        - Забирает все Submission с тем же AssignmentId
        - Применяет алгоритм: Плагиат, если существует более ранняя сдача того же задания с тем же ContentHash
        - Возвращается AnalyzeResultDto:
     ```
          {
          "submissionId": "...",
          "similarity": 0 или 1,
          "isPlagiarism": true/false,
          "details": "..."
          }
   4.3 Сценарий 3: преподаватель просматривает отчёты по всему заданию
   Как вызывается: GET /assignments/{assignmentId}/reports 
   - Технический обмен данными 
       - Check Service получает все Submission по заданию
       - Для каждого Submission выполняется тот же алгоритм плагиата, что и выше
       - Возвращается список отчётов:
     ```
         [
         {
         "submissionId": "...",
         "similarity": 1,
         "isPlagiarism": true,
         "details": "same file content submitted earlier"
         },
         ...
         ]
   4.4 Сценарии работы только с StorageService:
   - Загрузка файла в хранилище 
     - Запрос: POST /files 
     - Параметры формы:
         - file - загружаемый файл 
     - Что делает Storage Service:
          - Генерирует новый GUID -> это будущий FileId
          - Запоминает оригинальное имя файла
          - Сохраняет файл в файловой системе контейнера (в папке /app/uploads)
          - Создаёт запись о файле в SQLite
          - Возвращает метаданные файла
       ```
            {
            "fileId": "8e7f2fcd-4b44-4a6e-a4c9-2c3c1a41d1c8",
            "fileName": "my_document.txt",
            "size": 123456,
            "uploadedAt": "2025-12-10T20:14:31.123Z"
            }
   - Получение файла по идентификатору
     - Запрос: GET /files/{id} Где {id} - fileId, полученный при загрузке
     - Что делает Storage Service
         - Ищет запись в SQLite
         - Проверяет существование файла на диске
         - Если найден - читает файл и возвращает поток
         - Если не найден - возвращает 404



5. Реализация критериев
- Микросервисная архитектура состоящая из двух микросервисов + API gateway описана выше
- Обработка ошибок и отказоустойчивость микросервисов:
    - При создании сдачи (POST /submissions) сервис проверки (Check Service) взаимодействует с сервисом хранения файлов (Storage Service) по HTTP. Если Storage Service недоступен или возвращает ошибку, HTTP-клиент HttpFileStorageClient генерирует специальное исключение StorageUnavailableException.
    - Это исключение перехватывается на уровне контроллера SubmissionsController, после чего клиенту возвращается корректный HTTP-ответ:
    ``` 503 Service Unavailable
      {
      "error": "Storage service is unavailable. Please try again later."
      }

- Контейнеризация:
    - Каждый сервис имеет свой Dockerfile (проект Host).
    - docker-compose.yml поднимает всю систему автоматически.
    - Сервисы соединены через внутреннюю сеть.
    - SQLite и файлы вынесены в Docker volumes.
- Swagger:
   - Каждый сервис имеет свой Swagger (/swagger)
   - Поднят единый UI документирования - Scalar (/scalar)



