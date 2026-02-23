% Read CSV file
data = readtable('foot_rel_legbase.csv');

% Extract columns
t  = data{:,1};   % time
x  = data{:,2};   % relative x
y  = data{:,3};   % relative y
z  = data{:,4};   % relative z

% 3D trajectory plot
figure;
plot3(x, y, z, 'LineWidth', 1.8);
grid on;
axis equal;

% xlim([-0.6 -0.35]);
% ylim([0.3 0.45]);
% zlim([0.3 0.6]);

xlabel('x_{rel} [m]');
ylabel('y_{rel} [m]');
zlabel('z_{rel} [m]');
title('Foot Tip Trajectory Relative to Leg Base');