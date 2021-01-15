export class MessageDto {
    public id?: number = null;
    public userId?: number = null;
    public friendId?: number = null;
    public messageType?: number = null;
    public message: string = '';
    public createdOn?: Date = null;
    public showButton: boolean = false;
  }