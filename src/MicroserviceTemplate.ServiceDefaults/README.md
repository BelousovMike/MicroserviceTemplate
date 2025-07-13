## Service Defaults Project

В этот проект добавляются методы расширений для подключения сервисов в DI.

Например:
`public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder`
или
`public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder`