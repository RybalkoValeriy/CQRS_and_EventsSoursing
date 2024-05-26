CREATE USER my_user_name
WITH PASSWORD 'postgresPsw';

CREATE DATABASE postgres;
GRANT ALL PRIVILEGES ON DATABASE postgres TO postgres;