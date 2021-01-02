export interface IUserInfo {
    id: number,
    email: string,
    firstName: string,
    lastName: string
  }
  export interface IUserInfoDto extends IUserInfo{
    fullName: string,
    createdOn?: Date
  }
  export class UserInfo implements IUserInfo {
      constructor(
        public id: number,
        public email: string,
        public firstName: string,
        public lastName: string
      ){}
  }