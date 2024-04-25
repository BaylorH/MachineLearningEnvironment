import pandas as pd
from sklearn.preprocessing import LabelEncoder
from sklearn.model_selection import train_test_split 
import torch
import torch.nn as nn
import torch.optim as optim
import torch.nn.functional as F
from torch.utils.data import Dataset, DataLoader


torch.manual_seed(42)

df = pd.read_csv("iris.csv")

# take a look at the csv file yourself first
# columns Sepal Length, Sepal Width, Petal Length, and Petal Width are input features and column Iris Type is target value
x = df[['Sepal Length', 'Sepal Width', 'Petal Length', 'Petal Width']].values
y = df['Iris Type']

# Binary encoding of labels
encoder = LabelEncoder()
encoder.fit(y)
y = encoder.transform(y)

train_x, test_x, train_y, test_y = train_test_split(x, y, train_size=.95, shuffle=True)

train_x = torch.tensor(train_x, dtype=torch.float32)
train_y = torch.tensor(train_y, dtype=torch.int64)

class IrisDataset(Dataset):
    def __init__(self, train_x, train_y):
        super(Dataset, self).__init__()
        self.x = train_x
        self.y = train_y

    def __len__(self):
        return len(self.x)

    def __getitem__(self, idx):
        return self.x[idx], self.y[idx]
    
hidden_size = 5
num_layers = 2
learning_rate = .001
epoch_size = 10

train_dataset = IrisDataset(train_x, train_y)
train_loader = DataLoader(train_dataset)

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
        
        
net = Net(train_x.shape[1], hidden_size, num_layers)

# loss function is nn.MSELoss since it is regression task
criterion = nn.CrossEntropyLoss()

#using Adam as optimizer
optimizer = optim.Adam(net.parameters(), lr=learning_rate)

net.train()
for epoch in range(epoch_size): # starting with 10 epochs
    for batch_idx, data in enumerate(train_loader):
        # get inputs and target values from dataloaders and move to device
        inputs, labels = data

        # zero the parameter gradients using zero_grad()
        optimizer.zero_grad() 
        # forward -> compute loss -> backward propogation -> optimize 
        outputs = net(inputs)
        loss = criterion(outputs, labels)
        loss.backward()
        optimizer.step()

torch.save(net.state_dict(), 'iris_classification_model.pt')

