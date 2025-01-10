# Utiliser une image de base .NET
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Copier les fichiers de l'application
COPY . ./

# Publier l'application
RUN dotnet publish -c Release -o out

# Définir le point d'entrée
ENTRYPOINT ["dotnet", "out/VotreApplication.dll"]