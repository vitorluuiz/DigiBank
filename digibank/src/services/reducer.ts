const reducer = (state: any, action: any) => {
  switch (action.type) {
    case 'update':
      return { ...state, count: state.count + 1 };
    default:
      return state;
  }
};

export default reducer;
