# React + TypeScript + Vite

```
npm create vite@latest client-store -- --template react-ts

cd client-store
npm install
npm run dev

docker build -t storepd322-client .

docker run -it --rm -p 5092:80 --name storepd322-client_container storepd322-client
docker run -d --restart=always --name storepd322-client_container -p 5092:80 storepd322-client
docker run -d --restart=always --name storepd322-client_container -p 5092:80 storepd322-client
docker run -d --restart=always --name storepd322-client_container -p 5092:80 storepd322-client
docker ps -a
docker stop storepd322-client_container
docker rm storepd322-client_container

docker images --all
docker rmi storepd322-client-api


 
docker login
docker tag storepd322-client:latest novakvova/storepd322-client:latest
docker push novakvova/storepd322-client:latest

docker pull novakvova/storepd322-client:latest
docker images --all
docker run -d --restart=always --name storepd322-client_container -p 5092:80 novakvova/storepd322-client

```

```nginx options /etc/nginx/sites-available/default

server {

	server_name   storepd322.itstep.click *.storepd322.itstep.click;
    location / {
       proxy_pass         http://localhost:5092;
       proxy_http_version 1.1;
       proxy_set_header   Upgrade $http_upgrade;
       proxy_set_header   Connection keep-alive;
       proxy_set_header   Host $host;
       proxy_cache_bypass $http_upgrade;
       proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
       proxy_set_header   X-Forwarded-Proto $scheme;
    }
}

sudo systemctl restart nginx
certbot
```


Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react/README.md) uses [Babel](https://babeljs.io/) for Fast Refresh
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react-swc) uses [SWC](https://swc.rs/) for Fast Refresh

## Expanding the ESLint configuration

If you are developing a production application, we recommend updating the configuration to enable type aware lint rules:

- Configure the top-level `parserOptions` property like this:

```js
export default tseslint.config({
  languageOptions: {
    // other options...
    parserOptions: {
      project: ['./tsconfig.node.json', './tsconfig.app.json'],
      tsconfigRootDir: import.meta.dirname,
    },
  },
})
```

- Replace `tseslint.configs.recommended` to `tseslint.configs.recommendedTypeChecked` or `tseslint.configs.strictTypeChecked`
- Optionally add `...tseslint.configs.stylisticTypeChecked`
- Install [eslint-plugin-react](https://github.com/jsx-eslint/eslint-plugin-react) and update the config:

```js
// eslint.config.js
import react from 'eslint-plugin-react'

export default tseslint.config({
  // Set the react version
  settings: { react: { version: '18.3' } },
  plugins: {
    // Add the react plugin
    react,
  },
  rules: {
    // other rules...
    // Enable its recommended rules
    ...react.configs.recommended.rules,
    ...react.configs['jsx-runtime'].rules,
  },
})
```
