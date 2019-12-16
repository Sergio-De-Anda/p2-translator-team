FROM mcr.microsoft.com/dotnet/core/sdk as build
WORKDIR /aspnet
COPY . .
RUN dotnet build
RUN dotnet publish --no-restore -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet
WORKDIR /dist
ENV ASPNETCORE_ENVIRONMENT=development
COPY --from=build /aspnet/out .
CMD ["dotnet", "P2Translator.Client.dll"]