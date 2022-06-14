
# Spbs

Spbs - or the Spbs Private Budget System - is a simple program that aims to aid you in tracking your everyday expenses. It will allow you to enter your purchases and track them over time using various graphs and statistics.

It is mostly for my own personal use, as I thought that making this app by myself would also help to keep me motivated enough to continue using it. However, I leave it here in the public domain under the MIT license, as a part of my digital portfolio, or if anyone else would like to try it out.

It is intended to be hosted in the cloud, but local hosting should also be possible as long as the required databases are set-up.

## Roadmap

### For version 1.0

- "Structured" and queryable location definitions (e.g., "Malmö/Malmö Central/O'Learys")
- Currency
- Timezone and date time formatting
- Tags and categories for purchases
- Monthly expenses
- Statistics and descriptives

### Going forward

- Receipt scanning (Azure OCR?)
- User-defined querys and graphs
- Integration with external services for up-to-date exchange rates
- Ability to track the user's own stocks and funds
- Performance measurement and optimizations

## Run Locally

The project is a Blazor server project that locally connects to two databases: an sql database for user management and a mongodb for the rest.

The neccessary databases are already configured in the docker-compose file, so before starting just run the following command from the WebUi project folder:

```bash
docker-compose up -d
```

When you are finished you can similarly run 

```bash
docker-compose down
```

The connection strings are defined in the appsettings file.

## Going Live

You can run the application "live" locally if you wish, by setting up the correct databases.

In the future I will configure a docker image so that the aplication can be deployed more easily.

I will also add a bicep template so that deploymet to Azure is smooth and automated.

## License

The source code is released under the MIT license.

[MIT](https://choosealicense.com/licenses/mit/)

