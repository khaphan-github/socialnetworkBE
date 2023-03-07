echo off
openssl genrsa -out private-2048.key 2048
pause
openssl rsa -in private-2048.key -pubout -out public-2048.key