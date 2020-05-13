import * as signalR from "@aspnet/signalr";

export abstract class SignalRService {

  protected connection: signalR.HubConnection = null;

  constructor(hubUrl: string) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl)
      .configureLogging(signalR.LogLevel.Information)
      .build();
  }

  public async getConnectionId(): Promise<string> {
    await this.checkStartConnection();
    return this.connection.invoke<string>("GetConnectionId");
  }

  async checkStartConnection() {
    if (this.connection.state === signalR.HubConnectionState.Disconnected) {
      await this.connection.start()
        .catch(error => {
          console.log(error);
          alert(error);
        });
    }
  }

}
