.PHONY: install update run

install:
	git clone https://semnome017@dev.azure.com/semnome017/semnome017/_git/ativos-frontend ../ativos-frontend
	git clone https://semnome017@dev.azure.com/semnome017/semnome017/_git/ativos-api ../ativos-api
	git clone https://semnome017@dev.azure.com/semnome017/semnome017/_git/ativos-migrations ../ativos-migrations
	git clone https://semnome017@dev.azure.com/semnome017/semnome017/_git/ativos-common ../ativos-common
	git clone https://semnome017@dev.azure.com/semnome017/semnome017/_git/bolsa-calculos-ativos ../bolsa-calculos-ativos
	git clone https://semnome017@dev.azure.com/semnome017/semnome017/_git/bolsa-nasdaq-precificacao ../bolsa-nasdaq-precificacao
	git clone https://semnome017@dev.azure.com/semnome017/semnome017/_git/reguladora-calculos-indices ../reguladora-calculos-indices
	git clone https://semnome017@dev.azure.com/semnome017/semnome017/_git/reguladora-sec-arquivos ../reguladora-sec-arquivos
	git clone https://semnome017@dev.azure.com/semnome017/semnome017/_git/reguladora-sec-rss ../reguladora-sec-rss
	git clone https://semnome017@dev.azure.com/semnome017/semnome017/_git/reguladora-sec-historico ../reguladora-sec-historico

develop:
	
update:

infra:
	docker-compose -f docker-compose.yml stop
	docker-compose -f docker-compose.yml build
	docker-compose -f docker-compose.yml up
	
run:
