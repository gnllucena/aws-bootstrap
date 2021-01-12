redis-server --daemonize yes && sleep 1;
redis-cli < /usr/local/etc/redis/seed.redis;
redis-cli save;
redis-cli shutdown;
redis-server --include /usr/local/etc/redis/redis.conf;