# syntax=docker/dockerfile:1
ARG NODE_VERSION=22.20.0
ARG PNPM_VERSION=10.14.0

################################################################################
# ЭТАП 1: СБОРКА (Твой код)
FROM node:${NODE_VERSION}-alpine as build
WORKDIR /usr/src/app

# Устанавливаем pnpm
RUN --mount=type=cache,target=/root/.npm \
    npm install -g pnpm@${PNPM_VERSION}

# Копируем конфиги и ставим зависимости
COPY package.json pnpm-lock.yaml ./
RUN pnpm install --frozen-lockfile

# Копируем код и собираем проект
COPY . .
RUN pnpm run build
# В итоге у нас появляется папка /usr/src/app/dist

################################################################################
# ЭТАП 2: ЗАПУСК (Nginx) - Это мы меняем для оптимизации
FROM nginx:alpine

# Копируем собранный React из предыдущего этапа в папку Nginx
COPY --from=build /usr/src/app/dist/spa /usr/share/nginx/html

# Настройка для SPA и проксирования API
RUN echo 'server { \
    listen 80; \
    root /usr/share/nginx/html; \
    index index.html; \
    \
    location / { \
        try_files $uri $uri/ /index.html; \
    } \
    \
    location /api/ { \
        proxy_pass http://api:8080; \
        proxy_http_version 1.1; \
        proxy_set_header Upgrade $http_upgrade; \
        proxy_set_header Connection "upgrade"; \
        proxy_set_header Host $host; \
        proxy_cache_bypass $http_upgrade; \
    } \
}' > /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]