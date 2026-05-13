#include <stdio.h>
#include <mpi.h>
#include <algorithm>
#include <ctime>

#define GRN   "\x1B[32m"
#define RESET "\x1B[0m"
#define CYN   "\x1B[36m"

#define MASTER_RANK 0

#define MASTER_TAG 1
#define WORKER_TAG 2

#define M_SIZE 10
#define MICRO 1000000

#define NOT_ENOUGH_PROCESSES_NUM_ERROR 1

MPI_Status status;

int a[M_SIZE][M_SIZE];
int b[M_SIZE][M_SIZE];
int c[M_SIZE][M_SIZE];

int GenerateRandomNumber(){
	return std::rand() % 9 + 1;
}

template<int rows, int cols> 
void FillMatrix(int (&matrix)[rows][cols]){
	for(int i = 0; i < cols; i ++){
        for(int j = 0; j < rows; j ++){
          matrix[i][j] = GenerateRandomNumber();
        }
    }
}

template<int rows, int cols> 
void PrintMatrix(int (&matrix)[rows][cols]){
	printf("\n");
	for(int i = 0; i < rows; i ++){
		for(int j = 0; j < cols; j ++){
			printf("%d ", matrix[i][j]);
		}
		printf("\n");
	}
}


int main(int argc, char *argv[])
{
	int communicator_size;
	int process_rank;
	int process_id;
	int offset;
	int rows_num;
	int workers_num;
	int remainder;
	int whole_part;
	int message_tag;
	int i;
	int j;
	int k;


	MPI_Init(&argc, &argv);
	MPI_Comm_size(MPI_COMM_WORLD, &communicator_size);
	MPI_Comm_rank(MPI_COMM_WORLD, &process_rank);

	if(communicator_size < 2){
		MPI_Abort(MPI_COMM_WORLD, NOT_ENOUGH_PROCESSES_NUM_ERROR);
	}

	if(process_rank == MASTER_RANK){
		printf("%sGenerating matrixes%s\n", CYN, RESET);
		
		printf("\n%sGenerating matrix %sA%s with size %s%dx%d",CYN, GRN, CYN, RESET, M_SIZE, M_SIZE);
		FillMatrix(a);
		PrintMatrix(a);

		printf("\n%sGenerating matrix %sB%s with size %s%dx%d",CYN, GRN, CYN, RESET, M_SIZE, M_SIZE);
		FillMatrix(b);
		PrintMatrix(b);

		printf("\nStarting multiplication ... \n");
		long long int start = clock();

		workers_num = communicator_size - 1;
		whole_part = M_SIZE / workers_num;
		remainder = M_SIZE % workers_num;
		offset = 0;

		message_tag = MASTER_TAG;
		for(process_id = 1; process_id <= workers_num; process_id ++ ){
			rows_num = process_id <= remainder ? whole_part + 1 : whole_part;
			MPI_Send(&offset, 1, MPI_INT, process_id, message_tag, MPI_COMM_WORLD);
			MPI_Send(&rows_num, 1, MPI_INT, process_id, message_tag, MPI_COMM_WORLD);
			MPI_Send(&a[offset][0], rows_num * M_SIZE, MPI_INT, process_id, message_tag, MPI_COMM_WORLD);
			MPI_Send(&b, M_SIZE * M_SIZE, MPI_INT, process_id, message_tag, MPI_COMM_WORLD);

			offset += rows_num;
		}

		message_tag = WORKER_TAG;
		for(process_id = 1; process_id <= workers_num; process_id ++){
			MPI_Recv(&offset, 1, MPI_INT, process_id, message_tag, MPI_COMM_WORLD, &status);
			MPI_Recv(&rows_num, 1, MPI_INT, process_id, message_tag, MPI_COMM_WORLD, &status);
			MPI_Recv(&c[offset][0], rows_num * M_SIZE, MPI_INT, process_id, message_tag, MPI_COMM_WORLD, &status);
		}
		printf("\n%sResult %sA*B%s", CYN, GRN, RESET);
		PrintMatrix(c);
		long long int end = clock();
		double diff = (double)((end - start) / (1.0 * MICRO));

		printf("\n%dx%d - %f seconds\n", M_SIZE, M_SIZE, diff);
	} 
	
	if(process_rank != MASTER_RANK){
		message_tag = MASTER_TAG;
		MPI_Recv(&offset, 1, MPI_INT, MASTER_RANK, message_tag, MPI_COMM_WORLD, &status);
		MPI_Recv(&rows_num, 1, MPI_INT, MASTER_RANK, message_tag, MPI_COMM_WORLD, &status);
		MPI_Recv(&a, rows_num * M_SIZE, MPI_INT, MASTER_RANK, message_tag, MPI_COMM_WORLD, &status);
		MPI_Recv(&b, M_SIZE * M_SIZE, MPI_INT, MASTER_RANK, message_tag, MPI_COMM_WORLD, &status);

		for(k = 0; k < M_SIZE; k ++){
			for(i = 0; i < rows_num; i ++){
				c[i][k] = 0;
				for(j = 0; j < M_SIZE; j ++){
					c[i][k] += a[i][j] * b[j][k];
				}
			}
		}

		message_tag = WORKER_TAG;
		MPI_Send(&offset, 1, MPI_INT, MASTER_RANK, message_tag, MPI_COMM_WORLD);
		MPI_Send(&rows_num, 1, MPI_INT, MASTER_RANK, message_tag, MPI_COMM_WORLD);
		MPI_Send(&c, rows_num * M_SIZE, MPI_INT, MASTER_RANK, message_tag, MPI_COMM_WORLD);
	}

	MPI_Finalize();
	return 0;
}