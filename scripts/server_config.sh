# Copy over Apache server configuration
# Different server configuration files are used for the development server and the production server
# backend development/staging pipeline
if [ "$APPLICATION_NAME" == "reach-be-dev" ]
then
    sudo cp /home/ec2-user/Hourglass/scripts/server_staging.conf /etc/httpd/conf.d/server.conf
fi

# backend production/release pipeline
if [ "$APPLICATION_NAME" == "reach-be-prod" ]
then
    sudo cp /home/ec2-user/Hourglass/scripts/server_production.conf /etc/httpd/conf.d/server.conf
fi

# Start the Apache service
sudo systemctl start httpd.service
