#!/bin/bash
set -e

# Cambia a la carpeta del proyecto donde está tu DbContext
cd /src/DealNotifier.Infrastructure.Persistence

# Genera la migración
dotnet ef migrations add InitialCreate --context ApplicationDbContext

# Aplica la migración
dotnet ef database update --context ApplicationDbContext
