

% Read CSV
data = readtable('FrontRightJointLog.csv');

% Extract columns
targetYaw=data{:,2};
targetHip=data{:,4};
targetKnee=data{:,6};
realYaw = data{:,3};   % Unity RealYaw
realHip = data{:,5};   % Unity RealHip
realKnee=data{:,7};

figure;
hold on; grid on;

% Unity data
plot(targetYaw,-targetKnee,'y','LineWidth',2)
plot(realYaw/pi*180, -realKnee/pi*180, 'r', 'LineWidth', 2);

% MATLAB IK data
plot(theta1_2_all/pi*180, (theta4_2_all-theta3_2_all)/pi*180, 'b', 'LineWidth', 2);

xlabel('Yaw');
ylabel('Hip');

legend('Unity target','Unity (RealYaw vs -RealHip)', ...
       'MATLAB IK (theta1 vs theta3)');

title('Unity vs MATLAB Comparison (Yaw-Hip)');