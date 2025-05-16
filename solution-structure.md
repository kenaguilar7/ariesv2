# Aries Contabilidad Solution Structure

```
aries.contabilidad.2.0/
├── src/                              # Source code
│   ├── Core/                         # Core business logic
│   │   ├── Aries.Contabilidad.Core/  # Core domain models and interfaces
│   │   └── Aries.Contabilidad.Data/  # Data access and repositories
│   ├── Services/                     # Service layer
│   │   ├── Aries.WebAPI/            # REST API endpoints
│   │   └── Aries.WebServices/       # Additional services
│   └── UI/                          # User interface
│       └── Aries.Contabilidad/      # Desktop application
├── tests/                           # Test projects
│   ├── Unit/                        # Unit tests
│   └── Integration/                 # Integration tests
├── docs/                            # Documentation (GitHub Pages)
│   ├── api/                         # API documentation
│   ├── articles/                    # User guides and tutorials
│   ├── images/                      # Documentation images
│   ├── _templates/                  # Custom DocFX templates
│   ├── docfx.json                   # DocFX configuration
│   ├── index.md                     # Documentation home
│   └── toc.yml                      # Table of contents
├── tools/                           # Build and deployment tools
│   ├── build/                       # Build scripts
│   └── deploy/                      # Deployment scripts
├── .github/                         # GitHub specific files
│   ├── workflows/                   # GitHub Actions
│   └── ISSUE_TEMPLATE/             # Issue templates
├── config/                          # Configuration templates
│   ├── appsettings.json.example    # Example application settings
│   └── web.config.example          # Example web configuration
├── .gitignore                      # Git ignore rules
├── README.md                        # Project overview
└── Aries.sln                       # Solution file
```

## Key Features of This Structure

1. **Clear Separation of Concerns**
   - Core business logic isolated in Core/
   - Services separated from UI
   - Clear distinction between different layers

2. **Maintainable Documentation**
   - Centralized in docs/ folder
   - API documentation auto-generated
   - User guides and tutorials in articles/

3. **Development Tools**
   - Build and deployment scripts in tools/
   - GitHub workflows in .github/
   - Configuration templates in config/

4. **Testing**
   - Separate test projects by type
   - Clear organization of test resources

5. **Configuration Management**
   - Template configurations in config/
   - Examples provided for all config files
   - Sensitive configs ignored by Git 