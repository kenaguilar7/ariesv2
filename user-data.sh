#!/bin/bash

# Update system
yum update -y

# Install required packages
yum install -y docker git

# Start and enable Docker service
systemctl start docker
systemctl enable docker

# Add ec2-user to docker group
usermod -aG docker ec2-user

# Install Docker Compose
curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
chmod +x /usr/local/bin/docker-compose

# Create app directory
mkdir -p /app
cd /app

# Clone the repository
git clone https://github.com/kenaguilar7/ariesv2.git .

# Create a startup script
cat << 'EOF' > /app/start-app.sh
#!/bin/bash
cd /app
docker-compose up -d
EOF

chmod +x /app/start-app.sh

# Create systemd service for auto-start
cat << 'EOF' > /etc/systemd/system/aries-app.service
[Unit]
Description=Aries Application
After=docker.service
Requires=docker.service

[Service]
Type=oneshot
RemainAfterExit=yes
WorkingDirectory=/app
ExecStart=/app/start-app.sh
User=ec2-user

[Install]
WantedBy=multi-user.target
EOF

# Enable and start the service
systemctl enable aries-app
systemctl start aries-app

# Add a simple health check script
cat << 'EOF' > /app/health-check.sh
#!/bin/bash
curl -f http://localhost:5000/health || exit 1
curl -f http://localhost:8080 || exit 1
EOF

chmod +x /app/health-check.sh 