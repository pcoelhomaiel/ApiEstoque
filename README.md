# ApiEstoque

Passos, Criar o Banco (Console do Nuget):
add-migration "init" -Context UserDbContext
update-database -Context UserDbContext
add-migration "initialsetup" -Context AplicacaoDbContext
update-database -Context AplicacaoDbContext
