ALTER SESSION SET CONTAINER=XEPDB1;

CREATE USER HOPPER IDENTIFIED BY FlowMatic QUOTA UNLIMITED ON USERS;

GRANT CONNECT, RESOURCE TO HOPPER;