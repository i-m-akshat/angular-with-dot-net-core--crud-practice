export const claimReq = {
  adminOnly: (c: any) => c.role == 'Admin',
  adminOrTeacherOnly: (c: any) => c.role == 'Admin' || c.role == 'Teacher',
};
