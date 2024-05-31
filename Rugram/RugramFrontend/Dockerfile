FROM node:18.13.0-alpine

WORKDIR /app

COPY ./package.json .

RUN npm install

COPY . .

RUN npm install -g vite

RUN vite build

EXPOSE 80

# EXPOSE 443

CMD ["npm", "run", "dev" ]