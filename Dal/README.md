6)	Скрипты работы с базой данных выносить в DAL.
	Предлогаю использовать Npgsql EF Core provider — Npgsql.EntityFrameworkCore.PostgreSQL (обсуждаеться).
	нельзя использовать Entity Framework. Пишут что у Entity Framework проблемы с быстродействием на больших проектов. 
	ODBC – нельзя использовать, т.к. как заказчик потом хочет все на linux серверах использовать. 
