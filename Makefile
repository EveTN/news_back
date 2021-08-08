migrate:
	@dotnet ef database update -s ./Host/ -p ./Database/

migrate-add:
	@dotnet ef migrations add $(name) -s ./Host/ -p ./Database/

migrate-upd:
	@dotnet ef database update $(name) -s ./Host/ -p ./Database/

migrate-rm:
	@dotnet ef migrations remove -s ./Host/ -p ./Database/