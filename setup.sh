#!/bin/bash
MAX_RETRIES=5
RETRY_INTERVAL=5
retries=0
docker compose -f docker-compose.dev.yml up -d 
docker cp management.sql $(docker container ls -qf "name=mysql" | head -n1):/opt
while [ $retries -lt $MAX_RETRIES ]; do
    docker exec -it mysql bash -c "mysql -u root -pPassword1234 -S /var/run/mysqld/mysqld.sock fplms_management < /opt/management.sql"
    if [ $? -eq 0 ]; then
        echo "Setup script executed successfully."
        break
    else
        echo "Setup script failed. Retrying in $RETRY_INTERVAL seconds..."
        sleep $RETRY_INTERVAL
        retries=$((retries + 1))
    fi
done
if [ $retries -eq $MAX_RETRIES ]; then
    echo "Exceeded maximum number of retries. Setup script failed."
fi
