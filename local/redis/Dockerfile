FROM redis
COPY redis.conf /usr/local/etc/redis/
COPY seed.redis /usr/local/etc/redis/
COPY redis.sh /usr/local/etc/redis/
CMD ["sh", "/usr/local/etc/redis/redis.sh"]