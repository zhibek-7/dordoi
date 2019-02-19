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
    let startPromise: Promise<void> = new Promise<void>(resolve => resolve());
    if (this.connection.state === signalR.HubConnectionState.Disconnected) {
      startPromise = this.connection.start()
        .catch(error => {
          console.log(error);
          alert(error);
        });
    }
    await startPromise;
  }

}
