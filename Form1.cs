namespace SimpleCalculator
{
    public partial class Form1 : Form
    {
        // 계산을 위해 이전 숫자와 연산자를 기억할 변수
        private double firstOperand = 0;
        private string currentOperator = "";
        private bool isOperatorClicked = false;

        public Form1()
        {
            InitializeComponent();

            // 0 ~ 9 버튼을 하나의 클릭 이벤트에 연결
            BT0.Click += NumberButton_Click;
            BT1.Click += NumberButton_Click;
            BT2.Click += NumberButton_Click;
            BT3.Click += NumberButton_Click;
            BT4.Click += NumberButton_Click;
            BT5.Click += NumberButton_Click;
            BT6.Click += NumberButton_Click;
            BT7.Click += NumberButton_Click;
            BT8.Click += NumberButton_Click;
            BT9.Click += NumberButton_Click;

            // 연산자 및 = 버튼을 이벤트에 연결
            BTpl.Click += OperatorButton_Click;
            BTmi.Click += OperatorButton_Click;
            BTmulti.Click += OperatorButton_Click;
            BTdivision.Click += OperatorButton_Click;
            BTeq.Click += EqualButton_Click;

            // 지우기 버튼 이벤트 연결
            BTce.Click += BTce_Click;
            BTc.Click += BTc_Click;
            BTdel.Click += BTdel_Click;
        }

        // 숫자 버튼이 클릭되었을 때 실행될 공통 메서드
        private void NumberButton_Click(object? sender, EventArgs e)
        {
            // 클릭된 버튼을 가져옴
            Button? clickedButton = sender as Button;
            if (clickedButton != null)
            {
                // 입력창이 "0"이거나 연산자 버튼을 방금 누른 상태라면, 새로 누른 숫자로 덮어씁니다.
                if (txtInput.Text == "0" || isOperatorClicked)
                {
                    txtInput.Text = clickedButton.Text;
                    isOperatorClicked = false; // 새로운 숫자를 입력 중이므로 상태 해제
                }
                else
                {
                    // 그 외에는 기존 숫자에 이어 붙입니다.
                    txtInput.Text += clickedButton.Text;
                }
            }
        }

        // 연산자(+, -, X, %) 버튼을 눌렀을 때 실행될 메서드
        private void OperatorButton_Click(object? sender, EventArgs e)
        {
            Button? clickedButton = sender as Button;
            if (clickedButton != null && double.TryParse(txtInput.Text, out double parsedValue))
            {
                firstOperand = parsedValue;            // 현재 입력된 숫자를 기억
                currentOperator = clickedButton.Text;  // 어느 연산기호를 눌렀는지 기억
                isOperatorClicked = true;              // 연산기호를 눌렀음을 표시

                // 위에 있는 txtResult에 과정 보여주기
                txtResult.Text = $"{firstOperand} {currentOperator}";
            }
        }

        // = 버튼을 눌렀을 때 실행될 메서드
        private void EqualButton_Click(object? sender, EventArgs e)
        {
            if (double.TryParse(txtInput.Text, out double secondOperand))
            {
                double result = 0;

                // 기억해둔 연산자에 따라 계산 수행
                switch (currentOperator)
                {
                    case "+": result = firstOperand + secondOperand; break;
                    case "-": result = firstOperand - secondOperand; break;
                    case "X": result = firstOperand * secondOperand; break;
                    case "%":
                        if (secondOperand != 0)
                        {
                            result = firstOperand % secondOperand;
                        }
                        else
                        {
                            MessageBox.Show("0으로 나눌 수 없습니다.");
                            return;
                        }
                        break;
                    default:
                        return; // 눌려진 연산기호가 없으면 실행 중단
                }

                // 결과 표시
                txtResult.Text = $"{firstOperand} {currentOperator} {secondOperand} = {result}";
                txtInput.Text = result.ToString();

                // 계산이 끝났으므로 다음 숫자를 누르면 새로 입력되도록 설정
                isOperatorClicked = true;
            }
        }

        // CE (Clear Entry) 버튼 : 현재 입력 중인 숫자만 초기화
        private void BTce_Click(object? sender, EventArgs e)
        {
            txtInput.Text = "0";
        }

        // C (Clear) 버튼 : 모든 계산 상태 초기화
        private void BTc_Click(object? sender, EventArgs e)
        {
            txtInput.Text = "0";
            txtResult.Text = "";
            firstOperand = 0;
            currentOperator = "";
            isOperatorClicked = false;
        }

        // del 버튼 : 현재 입력된 숫자의 마지막 자리 지우기
        private void BTdel_Click(object? sender, EventArgs e)
        {
            if (txtInput.Text.Length > 0 && txtInput.Text != "0")
            {
                // 문자열의 맨 마지막 글자를 1개 잘라냄
                txtInput.Text = txtInput.Text.Substring(0, txtInput.Text.Length - 1);

                // 다 지워서 빈칸이 되거나, 음수 기호(-)만 남은 경우 0으로 되돌림
                if (txtInput.Text == "" || txtInput.Text == "-")
                {
                    txtInput.Text = "0";
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
