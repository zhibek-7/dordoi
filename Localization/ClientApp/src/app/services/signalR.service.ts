import * as signalR from "@aspnet/signalr";

export abstract class SignalRService {

  protected connection: signalR.HubConnection = null;

  constructor(hubUrl: string) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl)
      .configureLogging(signalR.LogLevel.Information)
      .build();
  }

}
