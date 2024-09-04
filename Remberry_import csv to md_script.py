import csv
from itertools import chain
import argparse
import datetime
import os

def extract_text_to_markdown(csv_file, num_columns):
    # 读取CSV文件
    with open(csv_file, 'r', encoding='utf-8') as file:
        reader = csv.reader(file)
        data = [row[0].split('\t') for row in reader]

    cut_columns = num_columns // 2

    # 将数据分成指定的列数
    data_chunks = [data[i:i+cut_columns] for i in range(0, len(data), cut_columns)]

    now = datetime.datetime.now()
    formatted_date = now.strftime('%Y-%m-%d_%H%M%S')
    formatted_path = os.path.dirname(os.path.abspath(__file__))
    final_path = os.path.join(formatted_path, formatted_date + '.md')

    # 将数据写入Markdown文件
    with open(final_path, 'w', encoding='utf-8') as file:
        # file.write('| ' + ' | '.join([f'Column {i+1}' for i in range(num_columns)]) + ' |\n')
        for i in range(num_columns):
            if i % 2 == 0:
                file.write('| Voc ')
            else:
                file.write('| Def ')
        file.write('|\n')
        file.write('| ' + ' | '.join(['---'] * num_columns) + ' |\n')
        
        try:
            for chunk in data_chunks:
                if (len(chunk) < cut_columns):
                    chunk += ['-','-'] * (cut_columns - len(chunk))
                row = ['{}'] * num_columns # 指定num)columns个{}，用于填充数据
                
                # chunk = [item for sublist in chunk for item in sublist]
                chunk = list(chain(*chunk))
                row = ' | '.join(row).format(*chunk)
                row = '| ' + row + ' |'
                file.write(row + '\n')
        except Exception as e:
            print(f"An error occurred: {e}")

if __name__ == '__main__':
    
    parser = argparse.ArgumentParser(description='Extract text from CSV and format it in Markdown')
    parser.add_argument('csv_file', help='Path to the input CSV file')
    parser.add_argument('-c', '--columns', type=int, default=6, help='Number of columns to create in the Markdown table')
    args = parser.parse_args()

    extract_text_to_markdown(args.csv_file, args.columns)
