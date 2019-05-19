import { IdentityState } from './identity.state';
import { IdentityActionsTypes } from '../actions/identity.actions';

export const initialState: IdentityState = {
  user: null,
  errors: {
    signInError: null,
    signInSilentError: null
  }
};

export function identityReducer(
  state = initialState,
  action: { type: IdentityActionsTypes, payload: any}
): IdentityState {
  console.log(action);
  switch (action.type) {
    case IdentityActionsTypes.GetUserSuccess:
      return {
        ...state,
        user: action.payload.user,
        errors: {
          signInError: null,
          signInSilentError: null
        }
      };

    case IdentityActionsTypes.SignInError:
      return {
        ...state,
        errors: {
          ...state.errors,
          signInError: {
            message: action.payload.message,
            name: action.payload.name,
            stack: action.payload.stack
          }
        }
      };

    case IdentityActionsTypes.SignInSilentError:
      return {
        ...state,
        errors: {
          ...state.errors,
          signInSilentError: {
            message: action.payload.message,
            name: action.payload.name,
            stack: action.payload.stack
          }
        }
      };

    default:
      return state;
  }
}
