
API - Postman

Cookies 
    Login

    curl --location 'http://localhost:5020/api/Account/Login' \
    --header 'Content-Type: application/json' \
    --data '{
        "username": "tnt",
        "password": "123456"
    }'


    No need login
    curl --location 'http://localhost:5020/api/location/GetValueAnonymous'

    Need login
    curl --location 'http://localhost:5020/api/Account/GetInforUser?userId=1'



JWT Token

    curl --location 'http://localhost:5020/api/Jwt/LoginJWT' \
    --header 'Content-Type: application/json' \
    --data '{
        "username": "tnt",
        "password": "123456"
    }'

    curl --location 'http://localhost:5020/api/JWT/GetInforUserJwt?userId=1' \
    --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW'