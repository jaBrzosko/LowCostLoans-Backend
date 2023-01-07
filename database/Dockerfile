FROM postgres:12 as db
WORKDIR /app
COPY ./scripts/init.sh /docker-entrypoint-initdb.d
COPY ./scripts/seed.sql ./scripts/seed.sql