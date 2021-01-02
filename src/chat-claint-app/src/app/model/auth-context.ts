import { SimpleClaim } from './simple-claim';

export class AuthContext {
  claims: SimpleClaim[];

  get isAdmin() {
    return !!this.claims && !!this.claims.find(c =>
      c.type === 'role' && c.value === 'Admin');
  }
}