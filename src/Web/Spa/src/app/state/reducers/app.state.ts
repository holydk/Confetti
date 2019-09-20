import { SharedState } from '@shared/state/reducers/shared.state';
import { IdentityState } from '@app/core/identity/state/reducers/identity.state';

export interface AppState {
    shared: SharedState;
    identity: IdentityState;
}
