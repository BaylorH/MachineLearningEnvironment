import zmq
import numpy as np
import torch
import torch.nn as nn
import torch.nn.functional as F

class Net(nn.Module):
    def __init__(self, input_feature_size, hidden_size, num_layers):
        super(Net, self).__init__()
        self.fc1 = nn.Linear(input_feature_size, hidden_size)
        self.fc2 = nn.Linear(hidden_size, num_layers)
        
    def forward(self, x):
        out = self.fc1(x)
        out = F.relu(out)
        out = self.fc2(out)
        
        return out

input_feature_size = 4
hidden_size = 5
num_layers = 2
model = Net(input_feature_size, hidden_size, num_layers)
model.load_state_dict(torch.load('iris_classification_model.pt'))

model.eval() # turn on evaluation model

context = zmq.Context()
socket = context.socket(zmq.REP)
socket.bind("tcp://*:5555")

while True:
    #  Wait for next request from client
    bytes_received = socket.recv(16)
    input_data = torch.tensor(np.frombuffer(bytes_received, dtype=np.float32), dtype=torch.float32)
    print(input_data)

    with torch.no_grad():
        output = model(input_data.unsqueeze(0))
        _, predicted = torch.max(output, 1)
        if predicted == 1:
            predictedMessage = "Iris-setosa"  
        else: 
            predictedMessage = "Iris-nonsetosa"
        print(predictedMessage)

    
    socket.send(bytes(predictedMessage, 'utf-8'))
