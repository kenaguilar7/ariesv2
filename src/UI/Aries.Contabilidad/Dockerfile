# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["src/UI/Aries.Contabilidad/Aries.Contabilidad.csproj", "UI/Aries.Contabilidad/"]
RUN dotnet restore "UI/Aries.Contabilidad/Aries.Contabilidad.csproj"

# Copy the rest of the files
COPY . .

# Publish the application
WORKDIR "/src/UI/Aries.Contabilidad"
RUN dotnet publish "Aries.Contabilidad.csproj" -c Release -o /app/publish

# Runtime stage
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Copy the published app
COPY --from=build /app/publish/wwwroot .

# Copy nginx configuration
COPY src/UI/Aries.Contabilidad/nginx.conf /etc/nginx/nginx.conf

# Expose port 80
EXPOSE 80

# Start nginx
CMD ["nginx", "-g", "daemon off;"] 