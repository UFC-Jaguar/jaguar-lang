#sudo apt-get install libboost-all-dev

g++ server.cpp BoostSerializeFuncionario.cpp -o server -lboost_serialization -lboost_system
g++ client.cpp BoostSerializeFuncionario.cpp -o client -lboost_serialization -lboost_system


./server &
sleep 0.01

./client

rm client server

