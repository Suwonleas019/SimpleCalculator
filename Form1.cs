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

            // 폼에서 키보드 입력을 먼저 받을 수 있도록 설정
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

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
                // 계산이 끝난 직후(= 기호가 있는 경우) 숫자를 누르면 전체 초기화
                if (txtResult.Text.Contains("="))
                {
                    txtResult.Text = "";
                    txtInput.Text = "0";
                    isOperatorClicked = false;
                }

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

                // 숫자를 누를 때마다 결과창(txtResult)에 식을 실시간으로 반영
                UpdateResultTextBox();
            }
        }

        // 연산자(+, -, X, %) 버튼을 눌렀을 때 실행될 메서드
        private void OperatorButton_Click(object? sender, EventArgs e)
        {
            Button? clickedButton = sender as Button;
            if (clickedButton == null) return;
            string op = clickedButton.Text;

            // 이미 '='로 계산이 끝난 상태에서 연산자를 또 누르면, 그 결과값부터 이어서 계산
            if (txtResult.Text.Contains("="))
            {
                txtResult.Text = txtInput.Text + op;
                isOperatorClicked = true;
                return;
            }

            // 방금 연산자를 눌렀는데 다른 연산자로 바꾸는 경우 (예: + 눌렀다가 - 누름)
            if (isOperatorClicked && txtResult.Text.Length > 0)
            {
                // 마지막 연산자 문자를 새 연산자로 교체
                txtResult.Text = txtResult.Text.Substring(0, txtResult.Text.Length - 1) + op;
                return;
            }

            // 첫 번째 숫자와 현재 연산자를 결과창에 표시
            // 이미 UpdateResultTextBox()에 의해 제일 마지막 숫자가 txtResult.Text에 반영되어 있습니다.
            // 따라서 txtResult.Text 끝에 연산자 문자 하나만 붙여주면 중복 입력이 안 됩니다.
            txtResult.Text += op;
            isOperatorClicked = true;
        }

        // txtResult 창에 현재 입력중인 전체 식을 보여주는 헬퍼 메서드
        private void UpdateResultTextBox()
        {
            // 아직 '='로 계산되지 않았고, 눌려진 연산자가 있으면 (혹은 없을 때도)
            if (!txtResult.Text.Contains("=") && !isOperatorClicked)
            {
                string currentFormula = txtResult.Text;

                // 마지막에 연산자가 있는지 확인하고, 그 위치까지만의 문자열을 구합니다.
                int lastOpIndex = currentFormula.LastIndexOfAny(new char[] { '+', '-', 'X', '%' });

                if (lastOpIndex >= 0)
                {
                    // 연산자까지만 보존하고 뒤에 붙어있던 숫자는 잘라낸 뒤, 새로 입력된 숫자로 대체
                    currentFormula = currentFormula.Substring(0, lastOpIndex + 1);
                }
                else
                {
                    // 연산자가 없으면 (단순 첫 숫자 입력 중이면) 비움
                    currentFormula = "";
                }

                // 입력창이 비어있으면(CE로 지워진 상태) 숫자 없이 연산자까지만 보여줌
                if (txtInput.Text == "")
                {
                    txtResult.Text = currentFormula;
                }
                else
                {
                    txtResult.Text = currentFormula + txtInput.Text;
                }
            }
            else if (!isOperatorClicked && txtResult.Text == "")
            {
                // 연산자가 없고 그냥 첫 숫자 입력중
                txtResult.Text = txtInput.Text;
            }
        }

        // = 버튼을 눌렀을 때 실행될 메서드
        private void EqualButton_Click(object? sender, EventArgs e)
        {
            if (txtResult.Text.Contains("=")) return; // 이미 계산된 경우 무시
            if (string.IsNullOrEmpty(txtResult.Text)) return; // 연산자 없이 숫자인 경우 무시

            // 식을 UpdateResultTextBox에서 실시간으로 txtResult.Text에 붙이고 있으므로
            // fullExpression은 곧 txtResult.Text 전체가 됩니다. (예: "2+5")
            string fullExpression = txtResult.Text;

            // 계산을 위해 특수기호 변환 및 나누기 시 소수점(double) 연산을 위한 처리
            string computeExpression = fullExpression.Replace("X", "*")
                                                     .Replace("%", "* 1.0 /")
                                                     .Replace("/", "* 1.0 /");

            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                var result = dt.Compute(computeExpression, "");

                // 무한대나 숫자가 아닌 값이 나오는 경우 (예: 0으로 나누기)
                if (result.ToString() == "∞" || result.ToString() == "NaN" || result.ToString() == "Infinity")
                {
                    MessageBox.Show("0으로 나눌 수 없습니다.");
                    txtInput.Text = "0";
                    txtResult.Text = "";
                }
                else
                {
                    // 식과 결과를 표시 (예: 2+5+10=17)
                    txtResult.Text = fullExpression + "=" + result.ToString();
                    txtInput.Text = result.ToString();
                }

                isOperatorClicked = true;
            }
            catch
            {
                MessageBox.Show("수식이 올바르지 않습니다.");
                txtInput.Text = "0";
                txtResult.Text = "";
            }
        }

        // CE (Clear Entry) 버튼 : 현재 입력 중인 숫자만 초기화
        private void BTce_Click(object? sender, EventArgs e)
        {
            txtInput.Text = "";

            // 방금 지운 숫자가 결과창(txtResult)에도 실시간으로 반영되게 업데이트
            UpdateResultTextBox();
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

                // 지운 내용도 txtResult에 실시간 반영
                UpdateResultTextBox();
            }
        }

        // 키보드 입력을 처리하는 메서드
        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            // 키보드가 눌렸을 때 매칭되는 버튼의 클릭 이벤트를 강제로 발생시킵니다
            bool keyHandled = true; // 처리된 키인지 확인

            switch (e.KeyCode)
            {
                // 숫자 키보드 및 텐키패드 숫자 처리
                case Keys.D0: case Keys.NumPad0: BT0.PerformClick(); break;
                case Keys.D1: case Keys.NumPad1: BT1.PerformClick(); break;
                case Keys.D2: case Keys.NumPad2: BT2.PerformClick(); break;
                case Keys.D3: case Keys.NumPad3: BT3.PerformClick(); break;
                case Keys.D4: case Keys.NumPad4: BT4.PerformClick(); break;
                case Keys.D5:
                case Keys.NumPad5:
                    if (e.Shift) BTdivision.PerformClick(); // Shift + 5 = '%'
                    else BT5.PerformClick();
                    break;
                case Keys.D6: case Keys.NumPad6: BT6.PerformClick(); break;
                case Keys.D7: case Keys.NumPad7: BT7.PerformClick(); break;
                case Keys.D8:
                case Keys.NumPad8:
                    if (e.Shift) BTmulti.PerformClick();   // Shift + 8 = '*'
                    else BT8.PerformClick();
                    break;
                case Keys.D9: case Keys.NumPad9: BT9.PerformClick(); break;

                // 연산자 처리
                case Keys.Add: BTpl.PerformClick(); break;
                case Keys.Subtract: case Keys.OemMinus: BTmi.PerformClick(); break;
                case Keys.Multiply: BTmulti.PerformClick(); break;
                case Keys.Divide: BTdivision.PerformClick(); break;

                // Shift + = 인 경우 '+' 처리, 그냥 = 인 경우 '=' 처리
                case Keys.Oemplus:
                    if (e.Shift) BTpl.PerformClick();
                    else BTeq.PerformClick();
                    break;

                // 엔터(=), 백스페이스(del), ESC(C), Delete(CE)
                case Keys.Enter: BTeq.PerformClick(); break;
                case Keys.Back: BTdel.PerformClick(); break;
                case Keys.Escape: BTc.PerformClick(); break;
                case Keys.Delete: BTce.PerformClick(); break;

                default:
                    keyHandled = false; // 매칭되는 키가 아닐 경우
                    break;
            }

            // 매칭되는 키를 처리했다면 기본 입력(중복 입력)이나 '띵' 소리가 나지 않게 차단합니다.
            if (keyHandled)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
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

        private void BT7_Click(object sender, EventArgs e)
        {

        }
    }
}
