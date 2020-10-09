FROM mysql:8.0

ADD ./seeds/01_stocks.sql /docker-entrypoint-initdb.d/
ADD ./seeds/02_stocks_country.sql /docker-entrypoint-initdb.d/
ADD ./seeds/03_identity.sql /docker-entrypoint-initdb.d/