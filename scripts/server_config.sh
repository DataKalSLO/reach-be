# Copy over Apache server configuration
sudo cp /home/ec2-user/Hourglass/scripts/server.conf /etc/httpd/conf.d/

# Start the Apache service
sudo systemctl start httpd.service
