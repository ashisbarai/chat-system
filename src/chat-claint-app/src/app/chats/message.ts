export class MessageDto {
    public id?: number = null;
    public sender?: number = null;
    public receiver?: number = null;
    public message: string = '';
    public receivedOn?: Date = null;
  }