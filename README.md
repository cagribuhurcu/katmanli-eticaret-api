# katmanli-eticaret-api
## Kullanılan Teknolojiler
- ASP.Net Core MVC
- ASP.Net Core WebAPI
- OOP
- Katmanlı Mimari
- Code First
- Generic Repository
- Crud İşlemleri
- Area
- MS SQL

## Kurulum
Öncelikle proje clone edildikten sonra Repositories katmanında Context klasörünün içerisinde bulunan StokProjectContext'e giriyoruz.
Context sınıfımız içerisinde OnConfiguring metodunu kendimize göre özelleştiriyoruz.
```
optionsBuilder.UseSqlServer("Server=DESKTOP-OEIFO1O\\CAGRISERVER; Database=StokProjectDB; Uid=sa; Pwd=1234;");
```
Sonrasında Package Manager Console açılarak update-database komutu yazılmalırıdr.
