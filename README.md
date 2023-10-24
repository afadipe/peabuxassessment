# Peabux Assessment

This project is built using .Net, Postgres DB, EntityFrameworkCore for backend and React for frontend

# Backend Peabux Assessment
 All backend code are in the folder  ### `backend` please on visual studio project solution using this directory
## Changing DB Connection String
The commented out connection string is to be uncommented if you want to run this application using container

If you running it from your local system kindly change the username and password of the present connection string then run the backend api
<img width="559" alt="ConnectionString png" src="https://github.com/afadipe/sleek-docker-series/assets/28296967/bbc13a48-4abc-4064-b2dc-fbe3c70e4d8c">

## Runing migration
- Running on the system:
Open package console in Visual studio code change the default project to point to "Peabux.Infrastructure" abd run the command 
below
### `update-database`
<img width="542" alt="runmigration" src="https://github.com/afadipe/SMSlive247Integrateion/assets/28296967/76a07b11-3a30-4a88-a3c3-0383fe6eb274">

- Running using Container: From the package console run the command below this will generate DB script needed. 
### `Script Migration`


###  Kindly Note
Project runs on localhost:62414/`
To access swagger on the api side, use http://localhost:62414/swagger/index.html
You can proceed to test on swagger

# Frontend Peabux Assessment
 All frontend code are in the folder  ### `frontend` please on visual studio code folder using this directory

Open the terminal and run the command below
```
npm install
npm start
```

# Creating the docker container
Navigate to the main project directory on your system on Command Prompt or anyone you are familiar with.Your need to have docker desktop installed and running abd run the command below
### `docker-compose up -d`

After running this command you should have this when you check docker desktop
![ac890210-1b79-4b80-ac4a-632fc1736392](https://github.com/afadipe/sleek-docker-series/assets/28296967/71cfbb9a-a076-4a1d-926c-4ae524c0e65f)

###  Setting up PgAdmin
click on the PgAdmin ui it should show the ui below.
![536cd4fe-baf3-47bb-9f20-9e8bd6fe9808](https://github.com/afadipe/sleek-docker-series/assets/28296967/90e3db62-dc53-4028-8466-4556cd0adeda)
then enter the username and password in the docker-compose-yml. The credential below
<img width="302" alt="dockercomposepwd" src="https://github.com/afadipe/sleek-docker-series/assets/28296967/3570de53-4941-4b98-a42c-f1908fb9123d">
Then right click on the servers on the left hand side and pick add server.
enter any Name in the General section

![c4f60148-c656-4aa6-91f4-e0d7fd83a2c7](https://github.com/afadipe/sleek-docker-series/assets/28296967/5dd68aeb-642f-4094-a6c9-78ebbb980e05)

Then after that click on the connection section and name the name of the db container as the host and the password in the db in the docker compose.
![416351ff-7ded-45ac-9804-ec8152c88b3d](https://github.com/afadipe/sleek-docker-series/assets/28296967/a0c8719c-9fc7-4ecd-b283-4a316ed0b536)


