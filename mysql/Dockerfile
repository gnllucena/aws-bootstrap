FROM mysql:8.0

ADD ./seeds/01_InitialScript.sql /docker-entrypoint-initdb.d/
ADD ./seeds/02_InsertCountry.sql /docker-entrypoint-initdb.d/