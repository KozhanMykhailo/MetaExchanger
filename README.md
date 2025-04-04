# Meta-exchanger
📌 Requirements

Before running the project, ensure you have the following installed:

.NET SDK 7.0+

Docker

Docker Compose

(Optional) Postman for testing API endpoints

How to run:

- Clone the repository 

- Open ..\MetaExchanger\MetaExchanger

- Run 'docker-compose up' in CMD or similar...

- Use Postman to run requests or similar, examples :
	- http://localhost:8080/api/v1/metaexchangers?OperationType=Buy&Amount=6

	- http://localhost:8080/api/v1/metaexchangers?OperationType=Sell&Amount=2
	
Also you can run locally through VS19+:
- Run mssql server(locally or in docker, check connection string)

- Open MetaExchanger.sln -> F5

- Open swagger: http://localhost:5229/swagger/index.html

![image](https://github.com/user-attachments/assets/1f33fb67-c708-4792-84b6-ad1c6db531cc)

