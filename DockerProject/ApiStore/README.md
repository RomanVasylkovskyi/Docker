# pd322-asp

Create docker hub repository - publish
```
docker build -t pd322-asp-api . 
docker run -it --rm -p 5817:8080 --name pd322-asp_container pd322-asp-api
docker run -d --restart=always --name pd322-asp_container -p 5817:8080 pd322-asp-api
docker run -d --restart=always -v d:/volumes/pd322-asp/images:/app/images --name pd322-asp_container -p 5817:8080 pd322-asp-api
docker run -d --restart=always -v /volumes/pd322-asp/images:/app/images --name pd322-asp_container -p 5817:8080 pd322-asp-api
docker ps -a
docker stop pd322-asp_container
docker rm pd322-asp_container

docker images --all
docker rmi pd322-asp-api

docker login
docker tag pd322-asp-api:latest novakvova/pd322-asp-api:latest
docker push novakvova/pd322-asp-api:latest

docker pull novakvova/pd322-asp-api:latest
docker ps -a
docker run -d --restart=always --name pd322-asp_container -p 5817:8080 novakvova/pd322-asp-api

docker run -d --restart=always -v /volumes/pd322-asp/images:/app/images --name pd322-asp_container -p 5817:8080 novakvova/pd322-asp-api


docker pull novakvova/pd322-asp-api:latest
docker images --all
docker ps -a
docker stop pd322-asp_container
docker rm pd322-asp_container
docker run -d --restart=always --name pd322-asp_container -p 5817:8080 novakvova/pd322-asp-api
```

```nginx options /etc/nginx/sites-available/default
server {
    server_name   dockerpd322.itstep.click *.dockerpd322.itstep.click;
    location / {
       proxy_pass         http://localhost:5817;
       proxy_http_version 1.1;
       proxy_set_header   Upgrade $http_upgrade;
       proxy_set_header   Connection keep-alive;
       proxy_set_header   Host $host;
       proxy_cache_bypass $http_upgrade;
       proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
       proxy_set_header   X-Forwarded-Proto $scheme;
    }
}

server {
		server_name   qubix.itstep.click *.qubix.itstep.click;
		root /var/dist;
		index index.html;

		location / {
			try_files $uri /index.html;
			#try_files $uri $uri/ =404;
		}
}

server {
		server_name   admin-qubix.itstep.click *.admin-qubix.itstep.click;
		root /var/admin-qubix.itstep.click;
		index index.html;

		location / {
			try_files $uri /index.html;
			#try_files $uri $uri/ =404;
		}
}

sudo systemctl restart nginx
certbot
```

/var/api-qubix.itstep.click/