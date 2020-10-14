FROM node:10.16.3-alpine
RUN apk update
RUN apk add curl
WORKDIR /app
COPY package.json .
RUN npm install
COPY . .
EXPOSE 4200 49153
CMD npm start